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

import socket
import time

class SocketUtils:
    """
    Various socket convenience functions
    """

    @staticmethod
    def connect (host: str, port: int, retries = 5, retrygap = 2, startup = None):
        """
        Create TCP/IP stream-based connection to given host / port, with retries

        :param host: hostname to connect to
        :param port: port #
        :param retries: number of retries to connect
        :param retrygap: time to sleep between retries in seconds
        :param startup: function to be called to start server if server is down (only called once)
        """
        for i in range(retries):
            try:
                client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                client.connect((host, port))
                return client
            except ConnectionRefusedError:
                client.close()
                if startup is not None and i == 0:
                    startup()
                elif i == retries-1:
                    raise
                else:
                    time.sleep(retrygap)


