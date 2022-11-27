from abc import ABC, abstractmethod
from argparse import Namespace

from ner_model_training.src.utils.path import get_data_path


class BaseTrainClassifier(ABC):
    def __init__(self, config: Namespace):
        self._config = config
        self._data_path = get_data_path(config)

    @abstractmethod
    def load_data(self):
        pass

    @abstractmethod
    def load_vectorizer(self, data):
        pass

    @abstractmethod
    def load_embeddings(self):
        pass

    @abstractmethod
    def load_model(self):
        pass

    @abstractmethod
    def train(self):
        pass

    @abstractmethod
    def save(self):
        pass
