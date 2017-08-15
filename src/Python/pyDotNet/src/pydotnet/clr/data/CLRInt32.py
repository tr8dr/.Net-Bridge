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

from pydotnet.clr.CLRMessage import CLRMessage
from pydotnet.clr.data.CLRInt64 import CLRInt64
from pydotnet.core.io import *



class CLRInt32 (CLRMessage):
    """
    Class for int32
    """

    def __init__(self, v = 0):
        super().__init__ (CLRInt32.typeFor(v), v)


    def serialize (self, sock: BinarySocketWriter):
        """
        Serialize message
        """
        super().serialize(sock)
        if self.mtype == CLRMessage.TypeInt32:
            sock.writeInt32 (self.value)
        else:
            sock.writeInt64 (self.value)


    def deserialize (self, sock: BinarySocketReader):
        """
        Deserialize message (called after magic & type read)
        """
        super().deserialize(sock)
        if self.mtype == CLRMessage.TypeInt32:
            self.value = sock.readInt32()
        else:
            self.value = sock.readInt64()


    @staticmethod
    def create (v: int):
        """
        Determine the appropriate int type based on size
        """
        if abs(v) < 2147483648:
            return CLRInt32(v)
        else:
            return CLRInt64(v)


    @staticmethod
    def typeFor (v = 0):
        if v is None:
            return CLRMessage.TypeInt32
        elif abs(v) <= 2147483648:
            return CLRMessage.TypeInt32
        else:
            return CLRMessage.TypeInt64




