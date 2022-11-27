from argparse import Namespace

import numpy as np
import tensorflow as tf
from keras.layers import Dense, LSTM, Bidirectional, Embedding, Input, Dropout, TimeDistributed, SpatialDropout1D
from keras.layers.merge import concatenate
from keras.models import Model
from src.data_preparation.vectorizer import BaseVectorizer
from src.trainer.models.base_classifier import BaseClassifier
from src.trainer.models.crf import CRF


class BiLSTM(BaseClassifier):
    """A Keras implementation of BiLSTM-CRF for sequence labeling.
    Guillaume Lample, Miguel Ballesteros, Sandeep Subramanian, Kazuya Kawakami, Chris Dyer.
    "Neural Architectures for Named Entity Recognition". Proceedings of NAACL 2016.
    https://arxiv.org/abs/1603.01360
    """

    def __init__(self, config: Namespace, embedding_mat: np.ndarray, vectorizer: BaseVectorizer):
        super(BiLSTM, self).__init__(config)
        self._embeddings: np.ndarray = embedding_mat
        self._vectorizer = vectorizer
        self._char_embeddings_dim: int = self._config.char_embeddings_dim
        self._word_embeddings_dim: int = self._config.embeddings_dim
        self._trainable_emb: bool = self._config.trainable_emb
        self._dropout_rate_emb: float = self._config.dropout_rate_emb
        self._char_lstm_size: int = self._config.char_lstm_size
        self._word_lstm_size: int = self._config.word_lstm_size
        self._char_vocab_size: int = len(self._vectorizer.char_vocab.word_index) + 1
        self._fc_dim: int = self._config.fc_dim
        self._dropout_rate: float = self._config.dropout_rate
        self._activation: str = self._config.activation
        self._use_char: bool = self._config.use_char
        self._use_crf: bool = self._config.use_crf
        self._learning_rate: float = self._config.learning_rate
        self._loss: str = self._config.loss
        self._num_classes: int = self._config.num_classes

    def build(self):
        # build word embedding
        word_ids = Input(batch_shape=(None, None), dtype='int32', name='word_input')
        inputs = [word_ids]

        word_embeddings = Embedding(input_dim=self._embeddings.shape[0],
                                    output_dim=self._embeddings.shape[1],
                                    mask_zero=True,
                                    weights=[self._embeddings],
                                    name='word_embedding')(word_ids)

        word_embeddings = SpatialDropout1D(rate=self._dropout_rate_emb)(word_embeddings)
        # build character based word embedding
        if self._use_char:
            char_ids = Input(batch_shape=(None, None, None), dtype='int32', name='char_input')
            inputs.append(char_ids)
            char_embeddings = Embedding(input_dim=self._char_vocab_size,
                                        output_dim=self._char_embeddings_dim,
                                        mask_zero=True,
                                        name='char_embedding')(char_ids)
            char_embeddings = TimeDistributed(Bidirectional(LSTM(self._char_lstm_size)))(char_embeddings)
            word_embeddings = concatenate([word_embeddings, char_embeddings])

        word_embeddings = Dropout(rate=self._dropout_rate)(word_embeddings)

        lstm = Bidirectional(LSTM(units=self._word_lstm_size, return_sequences=True))(word_embeddings)

        lstm = Dense(self._fc_dim, activation=self._activation)(lstm)

        if self._use_crf:
            crf = CRF(self._num_classes, sparse_target=False)
            loss = crf.loss_function
            output = crf(lstm)
        else:
            loss = self._loss
            output = Dense(self._num_classes, activation='softmax', name='output')(lstm)

        with tf.device('/gpu:0'):
            model = Model(inputs=inputs, outputs=output)

        model.compile(loss=loss, optimizer=self._optimizer)

        return model
