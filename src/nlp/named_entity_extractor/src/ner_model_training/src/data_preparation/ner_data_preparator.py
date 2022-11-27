import argparse
import collections
import json
import os
from typing import Dict

import pandas as pd
import tensorflow as tf
import yaml
from ner_model_training.src.data_preparation.data_model import BaseInputFeatures
from ner_model_training.src.data_preparation.dataset import TokenDataset
from ner_model_training.src.data_preparation.embedding import Embeddings
from ner_model_training.src.data_preparation.vectorizer import TokenVectorizer
from ner_model_training.src.utils.config import NerConfig, from_dict
from ner_model_training.src.utils.names import TRAIN_TSV, DEV_TSV, EMBEDDINGS_PKL, CONFIG_JSON, DEV_SIZE, TRAIN_SIZE
from tensorflow_core.core.example.feature_pb2 import Feature

TASK_CONFIG = {
    'ner':
        {'config': NerConfig,
         'dataset': TokenDataset,
         'vectorizer': TokenVectorizer},
}


class DataPreparationBase:

    @staticmethod
    def _load_config(config_file) -> argparse.Namespace:
        config_dict = yaml.load(open(config_file), Loader=yaml.SafeLoader)
        config_class = TASK_CONFIG[config_dict['name']]['config']
        return from_dict(config_dict, config_class)

    @staticmethod
    def create_from_config(config_file):
        raise NotImplementedError

    def __init__(self, config: argparse.Namespace):
        self.config = config
        assert self.config.name in TASK_CONFIG, \
            f"Config.name must be one of {list(TASK_CONFIG.keys())}"

    def load_vectorizer(self, data):
        print("Loading vectorizer...")
        vectorizer = TASK_CONFIG[self.config.name]['vectorizer'](self.config)
        x, y = zip(*[(xy.text, xy.label) for xy in data])
        vectorizer.fit(x, y)

        return vectorizer

    def load_embeddings(self, vocab):
        embeddings = Embeddings(self.config, vocab)
        embeddings.load()

        return embeddings

    def _config_dict(self) -> dict:
        return collections.OrderedDict((attr, self.config.__getattribute__(attr))
                                       for attr in dir(self.config)
                                       if not attr.startswith("__"))

    def _create_config_json(self, train_data_size, valid_data_size) -> str:
        conf_dict = self._config_dict()
        conf_dict[TRAIN_SIZE] = train_data_size
        conf_dict[DEV_SIZE] = valid_data_size
        return json.dumps(conf_dict)

    def _save_config(self, config_json, config_path):
        with open(config_path, mode='w') as f:
            f.write(config_json)

    def run(self) -> None:
        dataset = TASK_CONFIG[self.config.name]['dataset'](self.config)
        train_data, len_train_data = dataset.load_data(TRAIN_TSV)
        valid_data, len_valid_data = dataset.load_data(DEV_TSV)

        vectorizer = self.load_vectorizer(pd.concat([train_data, valid_data], axis=0))
        vectorizer.save(dataset.path)

        embeddings = self.load_embeddings(vectorizer.word_vocab)
        embeddings_path = os.path.join(dataset.path, EMBEDDINGS_PKL)
        embeddings.save(embeddings_path)

        config_json = self._create_config_json(len_train_data, len_valid_data)
        config_path = os.path.join(dataset.path, CONFIG_JSON)
        self._save_config(config_json, config_path)


class NerDataPreparation(DataPreparationBase):

    @staticmethod
    def create_from_config(config_file):
        config = NerDataPreparation._load_config(config_file)
        return NerDataPreparation(config)

    def add_all_features(self, features: BaseInputFeatures) -> Dict[str, Feature]:
        all_features = collections.OrderedDict()
        all_features['text_ids'] = tf.train.Feature(int64_list=tf.train.Int64List(value=features.text_id[0]))
        all_features['label_ids'] = tf.train.Feature(int64_list=tf.train.Int64List(value=features.label_id[0]))
        return all_features
