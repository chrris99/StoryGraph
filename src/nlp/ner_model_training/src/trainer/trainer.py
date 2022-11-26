import json
import os
from typing import Any, NoReturn, Tuple

import pandas as pd
import tensorflow as tf
from keras import backend
from src.data_preparation.dataset import TokenDataset
from src.data_preparation.embedding import Embeddings
from src.trainer.data_generator import DataGenerator
from src.utils.names import VECTORIZER_PKL, EMBEDDINGS_PKL, \
                            MODEL_DIR, CONFIG_JSON, DEV_TSV, TRAIN_TSV
from src.data_preparation.vectorizer import TokenVectorizer as IndexTransformer
from src.trainer.models.bi_lstm import BiLSTM as Classifier
from src.trainer.base_trainer import BaseTrainClassifier
from src.trainer.callbacks import F1score, set_early_stop


class TrainClassifier(BaseTrainClassifier):

    def __init__(self, config: Any, logger: Any):
        super(TrainClassifier, self).__init__(config)
        self.logger = logger
        self._data_stats = self.load_data_stats()
        self._dataset, self._train_dataset, self._validation_dataset = self.load_data()
        self._vectorizer = self.load_vectorizer(pd.concat([self._train_dataset, self._validation_dataset], axis=0))
        self._embeddings = self.load_embeddings()
        self.model = self.load_model()

    def __call__(self, mode: str) -> NoReturn:
        if mode == 'train':
            self.train()
            self.save()

    def load_data(self) -> Tuple[Any, Any, Any]:
        self.logger.info("Loading data")
        if self._config.name == 'ner':
            dataset = TokenDataset(self._config)
        else:
            raise ValueError(f"Unsupported task type {self._config.name}")

        train_dataset, len_train_data = dataset.load_data(TRAIN_TSV)
        validation_dataset, len_valid_data = dataset.load_data(DEV_TSV)

        return dataset, train_dataset, validation_dataset

    def load_vectorizer(self, data) -> Any:
        self.logger.info("Loading vectorizer")
        if self._config.name == 'ner':
            vectorizer = IndexTransformer(self._config)
            vectorizer_vocab = IndexTransformer(self._config).load(os.path.join(self._data_path, VECTORIZER_PKL))

            vectorizer.word_vocab.word_index = vectorizer_vocab[0].word_index
            vectorizer.char_vocab.word_index = vectorizer_vocab[1].word_index
            vectorizer._label_vocab = vectorizer_vocab[2]
        else:
            raise ValueError(f"Unsupported task type: {self._config.name}")
        return vectorizer

    def load_embeddings(self) -> Any:
        return Embeddings(self._config, self._vectorizer).load_pkl(os.path.join(self._dataset.path, EMBEDDINGS_PKL))

    def load_model(self) -> Any:
        self.logger.info("Loading model")
        return Classifier(self._config, self._embeddings, self._vectorizer).build()

    def train(self) -> NoReturn:
        self.logger.info("Starting train !!!")
        x_train, y_train = zip(*[(xy.text, xy.label) for xy in self._train_dataset])
        self.logger.info(f'Number of train instances: {len(x_train)}')

        train_batch = DataGenerator(x_train, y_train,
                                    self._config.batch_size,
                                    self._vectorizer.transform,
                                    shuffle=True)
        x_valid, y_valid = zip(*[(xy.text, xy.label) for xy in self._validation_dataset])
        self.logger.info(f'Number of validation instances: {len(x_valid)}')

        valid_batch = DataGenerator(x_valid, y_valid,
                                    self._config.batch_size,
                                    self._vectorizer.transform)

        f1_score = F1score(valid_batch, vectorizer=self._vectorizer, task=self._config.name)
        callbacks = [f1_score, set_early_stop(patience=self._config.es_patience)]

        self.model.fit_generator(generator=train_batch,
                                 validation_data=valid_batch,
                                 epochs=self._config.num_epochs,
                                 shuffle=True,
                                 callbacks=callbacks)

    def save(self) -> NoReturn:
        self.logger.info("Saving model")
        try:
            model_path = self._config.model_dir
        except:
            model_path = os.path.join(self._data_path, MODEL_DIR, str(1))

        if self._config.name == 'ner':
            signature = tf.saved_model.signature_def_utils.predict_signature_def(
                inputs={
                    "word_input": self.model.input[0],
                    "char_input": self.model.input[1],
                },
                outputs={'output': self.model.output})
        else:
            raise ValueError(f"Unsupported task type: {self._config.name}")

        builder = tf.saved_model.builder.SavedModelBuilder(model_path)
        builder.add_meta_graph_and_variables(
            sess=backend.get_session(),
            tags=[tf.saved_model.tag_constants.SERVING],
            signature_def_map={"serving_default": signature})
        builder.save()

    def load_data_stats(self):
        self.logger.info("Loading data stats")
        data_obj = os.path.join(self._data_path, CONFIG_JSON)
        with open(data_obj) as f:
            data_json = json.load(f)

        return data_json
