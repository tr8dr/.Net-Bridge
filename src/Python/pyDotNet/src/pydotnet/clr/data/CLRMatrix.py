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

import pandas as pd

from pydotnet.clr.CLRUtils import CLRUtils
from pydotnet.clr.CLRMessage import CLRMessage
from pydotnet.core.io import *


class CLRMatrix (CLRMessage):
    """
    Class for matrix (represented as DataFrame)
    """

    def __init__(self, v = []):
        super().__init__ (CLRMessage.TypeMatrix, v)


    def serialize (self, sock: BinarySocketWriter):
        """
        Serialize message
        """
        super().serialize(sock)

        mat = self.value
        shape = mat.shape

        ## data-frame has possible string index
        if mat.__class__ == pd.DataFrame:
            CLRUtils.serializeIndex (mat.index, sock)
            CLRUtils.serializeIndex (mat.columns, sock)
            sock.writeInt32(shape[0])
            sock.writeInt32(shape[1])
            data = mat.as_matrix().flatten('F')
            sock.writeFloat64Array(data, includelen=False)
        else:
            sock.writeInt32(0)
            sock.writeInt32(0)
            sock.writeInt32(shape[0])
            sock.writeInt32(shape[1])
            data = mat.flatten('F')
            sock.writeFloat64Array(data, includelen=False)


    def deserialize (self, sock: BinarySocketReader):
        """
        Deserialize message (called after magic & type read)
        """
        super().deserialize(sock)
        Ridx = CLRUtils.deserializeIndex(sock)
        Cidx = CLRUtils.deserializeIndex(sock)
        rows = sock.readInt32()
        cols = sock.readInt32()
        rdata = sock.readFloat64Array (len = rows*cols)

        mat = rdata.reshape(rows, cols, order='F')
        if Ridx is not None or Cidx is not None:
            self.value = pd.DataFrame (mat, index=Ridx, columns=Cidx)
        else:
            self.value = mat

