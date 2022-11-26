from abc import ABC, abstractmethod
from argparse import Namespace

from keras.optimizers import Adam


class BaseClassifier(ABC):
    def __init__(self, config: Namespace):
        self._config = config
        self._optimizer = Adam(self._config.learning_rate)

    @abstractmethod
    def build(self, *args):
        pass
