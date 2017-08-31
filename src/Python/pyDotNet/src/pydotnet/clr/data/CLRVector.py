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

from pydotnet.clr.CLRUtils import CLRUtils
from pydotnet.clr.CLRMessage import CLRMessage
from pydotnet.core.io import *

from pydotnet.core.utils.DataUtils import *


class CLRVector (CLRMessage):
    """
    Class for vectors (represented as a single column DataFrame)
    """

    def __init__(self, v = []):
        super().__init__ (CLRMessage.TypeVector, v)


    def serialize (self, sock: BinarySocketWriter):
        """
        Serialize message
        """
        super().serialize(sock)

        vec = self.value
        vtype = vec.__class__

        ## data-frame has possible string index
        if vtype == pd.DataFrame:
            CLRUtils.serializeIndex (vec.index, sock)
            vec = toRowVector(vec.values)
            sock.writeFloat64Array(vec)
        else:
            sock.writeInt32(0)
            vec = toRowVector(vec)
            sock.writeFloat64Array(vec)


    def deserialize (self, sock: BinarySocketReader):
        """
        Deserialize message (called after magic & type read)
        """
        super().deserialize(sock)
        idx = CLRUtils.deserializeIndex(sock)
        data = sock.readFloat64Array ()
        if idx is not None:
            self.value = pd.DataFrame (data, index=idx)
        else:
            self.value = data


