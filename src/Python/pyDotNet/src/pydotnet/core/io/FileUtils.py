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

import os.path
import gzip
import bz2


class FileUtils:

    @staticmethod
    def readStreamFor (filename: str, binary=True):
        """
        Create a read-only stream for given file

        :param filename: filename
        :param mode: file open mode
        :return: stream
        """
        filename = os.path.expanduser(filename)
        extension = filename.split('.')[-1]
        if extension == "gz":
            return gzip.GzipFile(filename, 'r')
        elif extension == "bz2":
            return bz2.BZ2File(filename, 'r')
        else:
            return open(filename, 'rb' if binary else 'r')


    @staticmethod
    def writeStreamFor (filename: str, binary=True):
        """
        Create a writeable stream for given file

        :param filename: filename
        :param mode: file open mode
        :return: stream
        """
        filename = os.path.expanduser(filename)
        extension = filename.split('.')[-1]
        if extension == "gz":
            return gzip.GzipFile(filename, 'w')
        elif extension == "bz2":
            return bz2.BZ2File(filename, 'w')
        else:
            return open(filename, 'wb' if binary else 'w')


