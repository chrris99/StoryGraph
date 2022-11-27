import numpy as np
from keras.callbacks import Callback, EarlyStopping
from seqeval.metrics import classification_report as cr_token
from seqeval.metrics import f1_score as f1_token


class F1score(Callback):

    def __init__(self, seq, vectorizer=None, task=None):
        super(F1score, self).__init__()
        self.seq = seq
        self.p = vectorizer
        self.task = task

    @staticmethod
    def get_lengths(y_true):
        lengths = []
        for y in np.argmax(y_true, -1):
            try:
                i = list(y).index(0)
            except ValueError:
                i = len(y)
            lengths.append(i)

        return lengths

    def on_epoch_end(self, epoch, logs={}):
        label_true = []
        label_pred = []
        score = None
        for i in range(len(self.seq)):
            x_true, y_true = self.seq[i]
            y_pred = self.model.predict_on_batch(x_true)
            lengths = None
            if self.task == 'ner':
                lengths = self.get_lengths(y_true)

            y_true = self.p.inverse_transform(y_true, lengths)
            y_pred = self.p.inverse_transform(y_pred, lengths)

            label_true.extend(y_true)
            label_pred.extend(y_pred)

        if self.task == 'ner':
            score = f1_token(label_true, label_pred, average='macro')
            print('val_fscore: {:04.2f}'.format(score * 100))
            print(cr_token(label_true, label_pred))
        else:
            raise ValueError(f"Unsupported task type: {self.task}")
        logs['val_fscore'] = score


def set_early_stop(patience):
    return EarlyStopping(
        monitor='val_loss',
        patience=patience,
        restore_best_weights=True)
