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


class BufferedReader:
    """
    Buffered stream with various convenient operations to read fundamental
    types:

        - strings (UTF-8)
        - integers
        - floating point
        - bytes
        - arrays
    """

    def __init__ (self, fp):
        self._fp = fp
        self._buffer = bytearray()
        self._basis = 0
        self._pos = 0
        self._len = 0
        self._eof = False

        self._Sint16 = struct.Struct("@h")
        self._Sint32 = struct.Struct("@i")
        self._Sint64 = struct.Struct("@l")
        self._Sint16 = struct.Struct("@H")
        self._Sfloat64 = struct.Struct("@d")


    @property
    def position (self):
        """
        Current position in the file
        """
        return self._fp.tell()


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


    def read (self, len) -> bytes:
        """
        read a buffer
        """
        if (self._pos + len) > self._len:
            self._replenish(len)

        Istart = self._pos
        Iend = Istart + len
        vec = self._buffer[Istart:Iend]
        self._pos = Iend

        return vec


    def seek (self, pos: int):
        """
        Seek to position in file
        """
        self._fp.seek (pos)
        self._pos = 0
        self._len = 0


    def find (self, seq: bytes) -> int:
        """
        Locate start of sequence
        """
        tolen = len(seq)
        while not self.isEOF():
            if (self._pos + tolen) > self._len:
                self._replenish(tolen)

            try:
                idx = self._buffer.index(seq, self._pos, self._len)
                self._pos = idx
                return self._basis + idx
            except ValueError:
                self._pos = self._len - tolen + 1

        return -1


    def close(self):
        """
        Close socket stream
        """
        try:
            self._fp.close()
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
        self._basis = len(remains) + self._fp.tell()

        ## read until we have enough
        read = 1
        while len(self._buffer) < n and read > 0:
            amount = max(n - len(self._buffer), 1024)
            piece = self._fp.read(amount)
            read = len(piece)
            self._buffer.extend (piece)

        self._len = len(self._buffer)


