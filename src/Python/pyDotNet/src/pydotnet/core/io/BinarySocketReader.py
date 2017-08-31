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

from socket import socket
import struct
import numpy


class BinarySocketReader:
    """
    Buffered socket stream with various convenient operations to read fundamental
    types:

        - strings (UTF-8)
        - integers
        - floating point
        - bytes
        - arrays
    """

    def __init__ (self, sock: socket):
        self._sock = sock
        self._buffer = bytearray()
        self._pos = 0
        self._len = 0
        self._eof = False

        self._Sint16 = struct.Struct("@h")
        self._Sint32 = struct.Struct("@i")
        self._Sint64 = struct.Struct("@l")
        self._Sint16 = struct.Struct("@H")
        self._Sfloat64 = struct.Struct("@d")


    def isEOF (self):
        """
        Determine whether is EOF
        """
        if self._eof:
            return True
        elif self._pos < self._len:
            return False
        else:
            self._replenish(1)
            self._eof = self._len == 0
            return self._eof


    def readByte (self) -> int:
        """
        read a byte
        """
        if (self._pos + 1) > self._len:
            self._replenish(1)

        v = self._buffer[self._pos]
        self._pos += 1
        return v


    def readString (self) -> str:
        """
        read a string
        """
        len = self.readInt32()
        if (self._pos + len) > self._len:
            self._replenish(len)

        Istart = self._pos
        Iend = Istart + len
        s = self._buffer[Istart:Iend].decode("UTF-8")
        self._pos = Iend
        return s


    def readInt16 (self) -> int:
        """
        read an int16
        """
        if (self._pos + 2) > self._len:
            self._replenish(2)

        (v,) = self._Sint16.unpack_from(self._buffer, self._pos)
        self._pos += 2
        return v


    def readInt32 (self) -> int:
        """
        read an int
        """
        if (self._pos + 4) > self._len:
            self._replenish(4)

        (v,) = self._Sint32.unpack_from(self._buffer, self._pos)
        self._pos += 4
        return v

    def readInt64 (self) -> int:
        """
        read an int64
        """
        if (self._pos + 8) > self._len:
            self._replenish(8)

        (v,) = self._Sint64.unpack_from(self._buffer, self._pos)
        self._pos += 8
        return v


    def readFloat64 (self) -> float:
        """
        read a float
        """
        if (self._pos + 8) > self._len:
            self._replenish(8)

        (v,) = self._Sfloat64.unpack_from(self._buffer, self._pos)
        self._pos += 8
        return v


    def readByteArray (self, len = None) -> bytes:
        """
        read a byte array
        """
        if len is None:
            len = self.readInt32()
        if (self._pos + len) > self._len:
            self._replenish(len)

        Istart = self._pos
        Iend = Istart + len
        vec = self._buffer[Istart:Iend]
        self._pos = Iend

        return vec


    def readFloat64Array (self, len = None) -> numpy.array:
        """
        read a float64 array
        """
        if len is None:
            len = self.readInt32()
        if (self._pos + len) > self._len:
            self._replenish(len)

        vec = numpy.zeros ((len,), dtype=numpy.float64)
        for i in range(0, len):
            vec[i] = self.readFloat64()

        return vec


    def readInt32Array (self, len = None) -> numpy.array:
        """
        read a int32 array
        """
        if len is None:
            len = self.readInt32()
        if (self._pos + len) > self._len:
            self._replenish(len)

        vec = numpy.zeros ((len,), dtype=numpy.int32)
        for i in range(0, len):
            vec[i] = self.readInt32()

        return vec


    def readInt64Array (self, len = None) -> numpy.array:
        """
        read a int64 array
        """
        if len is None:
            len = self.readInt32()
        if (self._pos + len) > self._len:
            self._replenish(len)

        vec = numpy.zeros ((len,), dtype=numpy.int64)
        for i in range(0, len):
            vec[i] = self.readInt64()

        return vec


    def readStringArray (self, len = None) -> list:
        """
        read a string array
        """
        if len is None:
            len = self.readInt32()
        if (self._pos + len) > self._len:
            self._replenish(len)

        vec = [self.readString() for i in range(0,len)]
        return vec


    def close(self):
        """
        Close socket stream
        """
        try:
            self._sock.close()
        finally:
            self._pos = 0


    def _replenish (self, n: int):
        """
        replenish's buffer to minimum required capacity (or more)
        """
        ## copy out remainder
        remains = self._buffer[self._pos:self._len]
        self._buffer.clear()
        self._buffer.extend(remains)
        self._pos = 0

        ## read until we have enough
        read = 1
        while len(self._buffer) < n and read > 0:
            amount = max(n - len(self._buffer), 1024)
            piece = self._sock.recv(amount)
            read = len(piece)
            self._buffer.extend (piece)

        self._len = len(self._buffer)


