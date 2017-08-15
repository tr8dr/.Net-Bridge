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
from pydotnet.clr.CLRTypeFactory import CLRTypeFactory


class CLRMessage:
    """
    Base class for all CLR messages
    """

    Magic                       = 0xd00d
    TypeNull					= 0
    TypeBool					= 1
    TypeByte					= 2
    TypeInt32					= 5
    TypeInt64					= 6
    TypeFloat64					= 7
    TypeString					= 8
    TypeObject					= 9

    TypeVector					= 21
    TypeMatrix					= 22
    TypeException				= 23

    TypeBoolArray				= 101
    TypeByteArray				= 102
    TypeInt32Array				= 105
    TypeInt64Array				= 106
    TypeFloat64Array			= 107
    TypeStringArray				= 108
    TypeObjectArray				= 109

    TypeCreate					= 201
    TypeCallStaticMethod		= 202
    TypeCallMethod				= 203
    TypeGetProperty				= 204
    TypeGetIndexedProperty		= 205
    TypeGetIndexed				= 206
    TypeSetProperty				= 207
    TypeGetStaticProperty		= 208
    TypeSetStaticProperty		= 209
    TypeProtect					= 210
    TypeRelease					= 211
    TypeTemplateReq				= 212
    TypeTemplateReply			= 213


    def __init__(self, mtype: int, value = None, api = None):
        self.mtype = mtype
        self.value = value
        self.api = api


    def serialize (self, sock: BinarySocketWriter):
        """
        Serialize message
        """
        self.api = sock.api
        ## magic number
        sock.writeInt16(CLRMessage.Magic)
        ## type
        sock.writeByte (self.mtype)


    def deserialize (self, sock: BinarySocketReader):
        """
        Deserialize message (called after magic & type read)
        """
        self.api = sock.api


    def toValue (self):
        """
        Present value of underlying
        """
        return self.value


    def __str__ (self) -> str:
        """
        Human readable form of message
        """
        return str(self.value)


    @staticmethod
    def read(sock: BinarySocketReader):
        """
        Read next message
        """
        magic = sock.readInt16()
        assert magic == CLRMessage.Magic
        mtype = sock.readByte()

        ## create obj
        obj = CLRTypeFactory.createById (mtype)
        ## deserialize
        obj.deserialize (sock)
        return obj


    @staticmethod
    def write (sock: BinarySocketWriter, msg):
        """
        Write out a message
        """
        msg.serialize (sock)
        sock.flush()







