import codecs
import os
import shutil
import tempfile

import joblib as jb
import numpy as np
from keras_preprocessing.sequence import pad_sequences
from keras_preprocessing.text import Tokenizer
from sklearn.base import TransformerMixin
from sklearn.preprocessing import LabelEncoder
from tensorflow_core.python.keras.utils import to_categorical
from src.utils.names import VECTORIZER_PKL, WORD_VOCAB, CHAR_VOCAB, LABEL_VOCAB


def pad_nested_sequences(sequences, max_sent_len, max_word_len, dtype='int32'):
    batch_max_sent_len = 0
    batch_max_word_len = 0
    for sent in sequences:
        batch_max_sent_len = max(len(sent), batch_max_sent_len)
        for word in sent:
            batch_max_word_len = max(len(word), batch_max_word_len)

    max_sent_len = min(batch_max_sent_len, max_sent_len)
    max_word_len = min(batch_max_word_len, max_word_len)

    x = np.zeros((len(sequences), max_sent_len, max_word_len)).astype(dtype)
    for i, sent in enumerate(sequences):
        sent = sent[:max_sent_len]
        for j, word in enumerate(sent):
            word = word[:max_word_len]
            x[i, j, :len(word)] = word

    return x


class BaseVectorizer(TransformerMixin):

    def __init__(self, config):
        self._config = config
        self._word_vocab = Tokenizer(lower=True, filters='', oov_token='UNK')
        self._label_vocab = LabelEncoder()
        self._max_len = self._config.max_seq_len
        self._num_classes = self._config.num_classes

    def fit(self, x, y):
        raise NotImplementedError()

    def transform(self, x, y=None):
        raise NotImplementedError

    def fit_transform(self, x, y=None, **params):
        return self.fit(x, y).transform(x, y)

    def inverse_transform(self, y, lengths):
        raise NotImplementedError

    @property
    def get_labels(self):
        return self._label_vocab.classes_

    @property
    def word_vocab(self):
        return self._word_vocab

    @property
    def get_labels_map(self):
        return dict(zip(self._label_vocab.classes_, range(len(self._label_vocab.classes_))))

    @property
    def get_rev_labels_map(self):
        return dict(zip(range(len(self._label_vocab.classes_)), self._label_vocab.classes_))

    @property
    def max_len(self):
        return self._max_len

    @property
    def num_classes(self):
        return self._num_classes

    def save(self, file_path: str, preprocessor):
        temp_dir = tempfile.gettempdir()

        temp_file_word = os.path.join(temp_dir, WORD_VOCAB)
        with codecs.open(os.path.join(temp_file_word), mode='w', encoding='utf-8') as f:
            for k, v in self._word_vocab.word_index.items():
                f.write(str(k) + '\t' + str(v) + '\n')

        temp_file_label = os.path.join(temp_dir, LABEL_VOCAB)
        with codecs.open(os.path.join(temp_file_label), mode='w', encoding='utf-8') as f:
            for k, v in zip(self._label_vocab.classes_, range(len(self._label_vocab.classes_))):
                f.write(str(v) + '\t' + (k) + '\n')

        jb.dump(preprocessor, os.path.join(file_path, VECTORIZER_PKL))
        shutil.move(temp_file_word, os.path.join(file_path, WORD_VOCAB))
        shutil.move(temp_file_label, os.path.join(file_path, LABEL_VOCAB))

    @classmethod
    def load(cls, file_path: str):
        p = jb.load(file_path)
        return p


class TokenVectorizer(BaseVectorizer):

    def __init__(self, config):
        super().__init__(config)
        if self._config.use_char:
            self._char_vocab = Tokenizer(lower=False, filters='', oov_token='UNK', char_level=True)
        self._max_word_len = self._config.max_word_len

    def fit(self, x, y):
        x_flat = [' '.join(t) for t in x]

        self._word_vocab.fit_on_texts(x_flat)

        vocab_size = sum([1 for _, i in self._word_vocab.word_counts.items() if i >= self._config.word_count_thresh])
        self._word_vocab.word_index = {e: i for e, i in self._word_vocab.word_index.items() if i <= vocab_size + 1}

        if self._config.use_char:
            self._char_vocab.fit_on_texts(x_flat)

        self._label_vocab.fit([z for t in y for z in t] + ['<pad>'])

        return self

    def transform(self, x, y=None):
        # Vectorize training and validation texts.
        x_flat, sent_lens = zip(*[(' '.join(t), len(t)) for t in x])
        max_sent_len = min(max(sent_lens), self._max_len)
        word_ids = self._word_vocab.texts_to_sequences(x_flat)
        word_ids = pad_sequences(word_ids, maxlen=max_sent_len, padding='post', truncating='post')

        if self._config.use_char:
            char_ids = [self._char_vocab.texts_to_sequences(sent) for sent in x]
            char_ids = pad_nested_sequences(char_ids, self._max_len, self._max_word_len)
            features = [word_ids, char_ids]
        else:
            features = word_ids

        if y is not None:
            y_enc = [self._label_vocab.transform(l).tolist() for l in y]
            y_enc = pad_sequences(y_enc, maxlen=max_sent_len, padding='post', truncating='post')
            y_enc = to_categorical(y_enc, len(self._label_vocab.classes_)).astype(int)

            return features, y_enc
        else:
            return features

    def inverse_transform(self, y, lengths):
        y = np.argmax(y, -1)
        inverse_y = [self._label_vocab.inverse_transform(row)[:l].tolist() for row, l in zip(y, lengths)]

        return inverse_y

    @property
    def char_vocab(self):
        return self._char_vocab

    def save(self, file_path, **kwargs):
        temp_dir = tempfile.gettempdir()

        if self._config.use_char:
            super().save(file_path, [self._word_vocab, self._char_vocab, self._label_vocab])
            temp_file_char = os.path.join(temp_dir, CHAR_VOCAB)
            with codecs.open(os.path.join(temp_file_char), mode='w', encoding='utf-8') as f:
                for k, v in self._char_vocab.word_index.items():
                    f.write(str(k) + '\t' + str(v) + '\n')

            shutil.move(temp_file_char, os.path.join(file_path, CHAR_VOCAB))
        else:
            super().save(file_path, [self._word_vocab, self._label_vocab])