import codecs
import os
from argparse import Namespace

import joblib as jb
import numpy as np

from src.utils.path import get_local_data_path


DATA_DIR_PREFIX = os.path.abspath(os.path.join(os.path.join(os.path.dirname(__file__), os.pardir)))

from abc import ABC, abstractmethod
from typing import Dict


class BaseEmbeddings(ABC):
    def __init__(self, config: Namespace, vocab: Dict):
        self._config = config
        self._vocab = vocab

    @abstractmethod
    def save(self, file_path):
        pass

    @abstractmethod
    def load(self):
        pass


class Embeddings(BaseEmbeddings):
    def __init__(self, config: Namespace, vocab):
        super(Embeddings, self).__init__(config, vocab)
        self.data_path = get_local_data_path(config)
        self.embeddings = None

    def load(self):
        print("Loading embeddings ...")
        embedding_matrix_all = {}

        raw_embeddings_local_file = os.path.join(self.data_path, self._config.embeddings_name)

        with codecs.open(raw_embeddings_local_file, encoding='utf-8') as f:
            for line in f:
                values = line.rstrip().split(' ')
                if len(values[1:]) > 1:
                    word = values[0]
                    embedding_matrix_all[word] = np.asarray(values[1:], dtype='float32')

        embedding_matrix = np.random.uniform(-0.05, 0.05, (len(self._vocab.word_index) + 1, self._config.embeddings_dim))

        missing_words = []
        for word, i in self._vocab.word_index.items():
            embedding_vector = embedding_matrix_all.get(word)
            if embedding_vector is not None:
                # words not found in embedding index will be random vector.
                embedding_matrix[i] = embedding_vector
            else:
                missing_words.append(word)

        print("Missing words count: {}".format(len(missing_words)))
        print("Missing words fraction: {}".format(len(missing_words)/len(self._vocab.word_index)))
        print(missing_words[:100])

        self.embeddings = embedding_matrix

    def save(self, file_path):
        jb.dump(self.embeddings, file_path)

    @classmethod
    def load_pkl(cls, file_path: str):
        e = jb.load(file_path)
        return e
