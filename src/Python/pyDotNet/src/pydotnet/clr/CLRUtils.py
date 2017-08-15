#
# General:
#      This file is pary of .NET Bridge
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

from pydotnet.core.io import *

import numpy as np
import pandas as pd


class CLRUtils:
    """
    Various utilities for CLR serialization
    """

    @staticmethod
    def serializeIndex (idx: pd.Index, sock: BinarySocketWriter):
        if idx.dtype == np.object or idx.dtype == np.str:
            vlen = len(idx)
            sock.writeInt32(vlen)
            for i in range(0,vlen):
                sock.writeString(str(idx[i]))
        else:
            sock.writeInt32(0)


    @staticmethod
    def deserializeIndex (sock: BinarySocketReader):
        ilen = sock.readInt32()
        if ilen > 0:
            return [sock.readString() for i in range(0,ilen)]
        else:
            return None


    @staticmethod
    def isSequence (obj) -> bool:
        return (not hasattr(obj, "strip")) and (hasattr(obj, "__getitem__") or hasattr(obj, "__iter__"))


    @staticmethod
    def shapeFor (obj):
        """
        Dimensions of object
        """
        if hasattr(obj, "shape"):
            shape = obj.shape
            return shape if len(shape) > 1 else shape[0]
        elif CLRUtils.isSequence(obj):
            return len(obj)
        else:
            return None


    @staticmethod
    def elementTypeFor (obj):
        """
        Determine most fundamental type for object (if array/list will return element type)
        """
        if hasattr(obj, "dtype"):
            return obj.dtype.type
        elif isinstance(obj, pd.DataFrame):
            return obj.values.dtype.type
        elif CLRUtils.isSequence(obj):
            return obj[0].__class__
        else:
            return obj.__class__



