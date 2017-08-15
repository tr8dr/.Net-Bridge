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

from socket import socket
import struct
import numpy


class BufferedWriter:
    """
    Buffered stream with various convenient operations to write fundamental
    types:

        - strings (UTF-8)
        - integers
        - floating point
        - bytes
        - arrays
    """

    def __init__ (self, fp, buflen = 1024):
        self._fp = fp
        self._buffer = bytearray()
        self._pos = 0
        self._buflen = buflen

        self._Sint16 = struct.Struct("@h")
        self._Sint32 = struct.Struct("@i")
        self._Sint64 = struct.Struct("@l")
        self._Sint16 = struct.Struct("@H")
        self._Sfloat64 = struct.Struct("@d")


    def writeInt16 (self, v: int):
        """
        write an int16
        """
        if (self._pos + 2) > self._buflen:
            self.flush()

        self._buffer.extend (self._Sint16.pack(v))
        self._pos += 2


    def writeInt32 (self, v: int):
        """
        write an int
        """
        if (self._pos + 4) > self._buflen:
            self.flush()

        self._buffer.extend (self._Sint32.pack(v))
        self._pos += 4


    def writeByte (self, v: int):
        """
        write a byte
        """
        if (self._pos + 1) > self._buflen:
            self.flush()

        self._buffer.append (v)
        self._pos += 1


    def writeInt64 (self, v: int):
        """
        write a long
        """
        if (self._pos + 8) > self._buflen:
            self.flush()

        self._buffer.extend (self._Sint64.pack(v))
        self._pos += 8


    def writeFloat64 (self, v: float):
        """
        write a float64
        """
        if (self._pos + 8) > self._buflen:
            self.flush()

        self._buffer.extend (self._Sfloat64.pack(v))
        self._pos += 8


    def writeString (self, v: str):
        """
        write a string
        """
        slen = len(v)
        self.writeInt32 (slen)
        self._buffer.extend (v.encode("utf-8"))
        self._pos += slen


    def write (self, v: bytes):
        """
        write a byte array
        """
        self._buffer.extend (v)
        self._pos += slen


    def flush (self):
        """
        flush pending data to channel
        """
        self._fp.write (self._buffer)
        self._buffer.clear()
        self._pos = 0


    def close(self):
        """
        Close socket stream
        """
        try:
            self.flush()
            self._fp.close()
        finally:
            self._pos = 0

