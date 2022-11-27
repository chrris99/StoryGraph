import argparse
import os
from argparse import Namespace

import numpy as np
import tensorflow as tf
from ner_model_training.src.utils.config import from_yaml_auto, to_dict
from ner_model_training.src.utils.names import VECTORIZER_PKL
from ner_model_training.src.data_preparation.vectorizer import TokenVectorizer as IndexTransformer
from ner_model_training.src.utils.path import get_data_path
from tensorflow_core.python.saved_model.loader_impl import load


class Inference:
    def __init__(self, config_path):
        # Tensor names for graph input and output
        self._word_tensor_key = "word_input:0"
        self._char_tensor_key = "char_input:0"
        self._output_tensor_key = "crf_1/cond/Merge:0"
        self.text_input = None
        self.tokens = None
        self.prob = None
        self.tags = None
        self._config = from_yaml_auto(config_path)
        self._data_path = get_data_path(self._config)
        self._vectorizer = self.load_vectorizer()

    def load_vectorizer(self):
        print("Loading vectorizer !!!")
        vectorizer = IndexTransformer(self._config)

        vectorizer_vocab = IndexTransformer(self._config).load(os.path.join(self._data_path, VECTORIZER_PKL))

        vectorizer.word_vocab.word_index = vectorizer_vocab[0].word_index
        vectorizer.char_vocab.word_index = vectorizer_vocab[1].word_index
        vectorizer._label_vocab = vectorizer_vocab[2]

        return vectorizer

    def predict(self):
        with tf.Session(graph=tf.Graph()) as sess:
            load(sess, export_dir=os.path.join(self._data_path), tags=[tf.saved_model.tag_constants.SERVING])
            graph = tf.get_default_graph()

            word = graph.get_tensor_by_name(self._word_tensor_key)
            char = graph.get_tensor_by_name(self._char_tensor_key)
            y = graph.get_tensor_by_name(self._output_tensor_key)

            pred = sess.run(y, feed_dict={word: self.text_input[0],
                                          char: self.text_input[1]})

            self.prob = np.max(pred, -1)
            self.tags = self._vectorizer.inverse_transform(pred, lengths=[len(self.tokens)])

    def transform(self, text: list):
        self.text_input, self.tokens = self._vectorizer.transform(text), text[0]

    def run(self, text: list):
        self.transform(text)
        self.predict()
        return self.tags


if __name__ == '__main__':
    config_path = '/Users/attilanagy/Personal/StoryGraph/src/nlp/named_entity_extractor/src/ner_model_training/data/ner/hu/1.0/train_ner.yml'

    tagger = Inference(config_path)

    tags = tagger.run(['Ez egy mondat amiben Ady Endre neve is szerepel.'.split()])
