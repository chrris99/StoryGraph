from typing import Optional, Union

import numpy as np


class BaseInputData:
    def __init__(self, text: Union[str, list], label: Optional[Union[str, list]] = None):
        self.text = text
        self.label = label

    def __iter__(self):
        yield self


class BaseInputFeatures:
    """A single training/test feature vector."""

    def __init__(self, text_id: np.ndarray, label_id: Optional[np.ndarray] = None):
        self.text_id = text_id
        self.label_id = label_id

    def __iter__(self):
        yield self


class TokenInputData(BaseInputData):
    def __init__(self, text, label):
        super(TokenInputData, self).__init__(text, label)


class TokenInputFeatures(BaseInputFeatures):
    def __init__(self, text_id, label_id):
        super(TokenInputFeatures, self).__init__(text_id, label_id)
