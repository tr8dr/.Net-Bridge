#
# General:
#      This file is part of .NET Bridge
#
# Copyright:
#      2010 Jonathan Shore
#      2017 Jonathan Shore and Contributors
#
# License:
#      Licensed under the Apache License, Version 2.0 (the "License");
#      you may not use this file except in compliance with the License.
#      You may obtain a copy of the License at:
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
#      Unless required by applicable law or agreed to in writing, software
#      distributed under the License is distributed on an "AS IS" BASIS,
#      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#      See the License for the specific language governing permissions and
#      limitations under the License.
#

import numpy as np
import pandas as pd
try:    
    from collections.abc import Iterable
except ImportError:    
    from collections import Iterable
from scipy.stats import *


def ncols(series):
    """
    Determine # of columns
    """
    if not hasattr(series, 'shape'):
        return 1
    if len(series.shape) == 1:
        return 1
    else:
        return series.shape[1]

def nrows(series):
    """
    Determine # of rows
    """
    if not hasattr(series, 'shape'):
        return len(series)
    else:
        return series.shape[0]


def breaks(series, mingap = 12*3600):
    """
    Get the row indices where there are at least k-second gaps

    :param series: series to determine breaks on
    :return: list of breaks
    """
    dt = np.diff(series.index.astype(np.int64)/1e9)

    isbreak = np.concatenate ((np.array([True]), dt[:-1] >= mingap, np.array([True])))
    indices, = np.where (isbreak)
    return indices


def toColumnVector(vec) -> np.array:
    """
    Convert vector to column vector if currently a row vector
    """
    if not hasattr(vec, 'shape'):
        vec = np.array(vec)

    shape = vec.shape
    if len(shape) == 1:
        return np.reshape (vec, (shape[0], 1))
    elif shape[0] == 1:
        return np.transpose(vec)
    else:
        return vec


def toRowVector(vec) -> np.array:
    """
    Convert vector to row vector if currently a column vector
    """
    if not hasattr(vec, 'shape'):
        return np.array(vec) if isinstance(vec, Iterable) else np.array([vec])
    else:
        return vec.flatten()


def cbind(*serieslist):
    """
    compose a list of column vectors into a matrix
    :param serieslist:
    :return:
    """
    vectors = [toColumnVector(vec) for vec in serieslist if vec is not None]
    return np.hstack(vectors)


def c(*serieslist):
    """
    concatenate a list of vectors
    """
    vectors = [toRowVector(vec) for vec in serieslist]
    return np.concatenate(vectors)


def summary(series):
    """
    Descriptive statistics for series
    """
    idx = ['mean', 'std', 'skew', 'kurtosis', 'min', '25%', 'median', '75%', 'max']

    def statistics (v):
        v = v[~np.isnan(v)]
        return np.array([
            np.mean(v), np.std(v), skew(v), kurtosis(v),
            np.min(v),
            np.percentile(v, 25), np.percentile(v, 50), np.percentile(v, 75),
            np.max(v)])

    if len(series.shape) == 1 or series.shape[1] == 1:
        return pd.DataFrame(statistics(series), index=idx, columns=None)

    data = None
    columns = series.columns
    for ci in columns:
        v = series[[ci]].values
        stats = statistics(v)
        snew = pd.DataFrame(stats, index=idx, columns=[ci])
        if data is None:
            data = snew
        else:
            data = pd.merge (data, snew, left_index=True, right_index=True, how='outer')

    return data




