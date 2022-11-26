import csv
import os
from abc import ABC, abstractmethod

import pandas as pd
from typing import Any, Dict
import tensorflow as tf
from src.utils.path import get_data_path
from src.data_preparation.data_model import TokenInputData


class BaseDataset(ABC):

    def __init__(self, config):
        self._config = config
        self.data_path = get_data_path(config)

    @abstractmethod
    def load_data(self, filename):
        pass

    def _load_data(self, filename):
        data = os.path.join(self.data_path, filename)
        return pd.read_csv(data, sep='\t', encoding='utf-8', quoting=csv.QUOTE_NONE)

    @property
    def path(self):
        return self.data_path


class TokenDataset(BaseDataset):
    def load_data(self, filename):

        df = self._load_data(filename)
        df.columns = ['sentence', 'token', 'tag']
        df.isna().values.any()
        df = df.dropna()
        df = df.reset_index(drop=True)
        df['token'], df['tag'] = df.groupby('sentence')['token'].apply(list), df.groupby('sentence')['tag'].apply(list)
        df = df.dropna()
        df = df.reset_index(drop=True)
        df['token_count'] = df['token'].apply(len)
        df = df[df['token_count'] > 1]
        data_model = df.apply(lambda x: TokenInputData(label=x['tag'], text=x['token']), axis=1)
        return data_model, len(df)


def flatten_rows(x):
    return x['token'].values, x['tag'].values


def get_feature_schema(task: str, config: Any) -> Dict:
    if task == 'ner':
        return {
            "token_ids": tf.io.FixedLenFeature([config.max_seq_len], tf.int64),
            "char_ids": tf.io.FixedLenFeature([config.max_seq_len * config.max_word_len], tf.int64),
            "label_ids": tf.io.FixedLenFeature([config.max_seq_len * config.num_classes], tf.int64),
        }
    else:
        raise ValueError("Unsupported task type")
