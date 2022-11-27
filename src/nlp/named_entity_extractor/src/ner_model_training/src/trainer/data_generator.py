import math

from keras.utils import Sequence


class DataGenerator(Sequence):

    def __init__(self, x, y, batch_size=1, preprocess=None, shuffle=False):
        self.x = x
        self.y = y
        self.batch_size = batch_size
        self.preprocess = preprocess
        self.shuffle = shuffle

    def __getitem__(self, idx):
        # Create and transform batches
        batch_x = self.x[idx * self.batch_size: (idx + 1) * self.batch_size]
        batch_y = self.y[idx * self.batch_size: (idx + 1) * self.batch_size]

        return self.preprocess(batch_x, batch_y)

    def __len__(self):
        return math.ceil(len(self.x) / self.batch_size)