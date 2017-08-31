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


class CLRTypeFactory:
    """
    CLR message factory
    """

    _shared = None

    def __init__(self):
        _shared = self
        self.byid = {}
        self.byobj = []


    def register (self,msg):
        """
        Register message
        """
        mclass = msg.__class__
        self.byid[msg.mtype] = lambda x: mclass(x)


    def add_obj_to_msg_factory (self, predicate, creator):
        """
        Register obj predicate -> msg creator
        """
        self.byobj.append((predicate, creator))


    @staticmethod
    def get():
        """
        Singleton
        """
        if CLRTypeFactory._shared is not None:
            return CLRTypeFactory._shared
        else:
            CLRTypeFactory._shared = CLRTypeFactory()
            return CLRTypeFactory._shared


    @staticmethod
    def createById (mtype, value = None):
        """
        Create object by messsage ID
        """
        registry = CLRTypeFactory.get()
        return registry.byid[mtype] (value)


    @staticmethod
    def createByValue (value):
        """
        Create object by value
        """
        registry = CLRTypeFactory.get()
        otype = value.__class__


    @staticmethod
    def createByValue (value):
        """
        Create object by value
        """
        registry = CLRTypeFactory.get()

        for factorypair in registry.byobj:
            pred,creator = factorypair
            if pred(value):
                return creator(value)

        raise Exception ("did not find factory for value: %s" % value.__class__)



