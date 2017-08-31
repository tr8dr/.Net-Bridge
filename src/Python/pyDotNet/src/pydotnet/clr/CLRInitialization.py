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

from pydotnet.clr.ctrl import *
from pydotnet.clr.data import *
from pydotnet.clr.CLRApi import CLRObject
from pydotnet.clr.CLRUtils import CLRUtils
from pydotnet.clr.CLRTypeFactory import CLRTypeFactory

import numpy as np



##
## value -> message mappings
##


def _create_matrix (shape, value):
    if isinstance(shape, int):
        return CLRVector (value)
    elif len(shape) == 1 or (shape[0] == 1 or shape[1] == 1):
        return CLRVector (value)
    else:
        return CLRMatrix (value)


_0D = {
    None.__class__: lambda x: CLRNull(),
    CLRObject: lambda x: CLRObjectRef(x.objectId),
    bool: lambda x: CLRBool(x),
    Exception: lambda x: CLRException(x),
    str: lambda x: CLRString(x),
    int: lambda x: CLRInt32(x),
    np.int32: lambda x: CLRInt32(x),
    np.int64: lambda x: CLRInt64(x),
    np.float: lambda x: CLRFloat64(x),
    np.float64: lambda x: CLRFloat64(x),
    float: lambda x: CLRFloat64(x)
}

_MD = {
    bool: lambda shape,x: CLRBoolArray(x),
    np.byte: lambda shape,x: CLRByteArray(x),
    str: lambda shape,x: CLRStringArray(x),
    np.object: lambda shape,x: CLRStringArray(x),
    np.object_: lambda shape,x: CLRStringArray(x),
    int: lambda shape,x: CLRInt32Array(x),
    np.int32: lambda shape,x: CLRInt32Array(x),
    np.int64: lambda shape,x: CLRInt64Array(x),
    float: _create_matrix,
    np.float64: _create_matrix
}

##
## predicates
##

def _is_object (obj):
    return issubclass(obj.__class__, CLRObject)

def _is_0d (obj):
    return obj.__class__ in _0D

def _is_otherwise (obj):
    return True


##
##  Factory functions
##

def _create_object (obj):
    return CLRObjectRef (obj.objectId)


def _create_0d (obj):
    return _0D[obj.__class__] (obj)


def _create_md (obj):
    ## determine whether a vector of matrix type
    shape = CLRUtils.shapeFor(obj)
    if shape is None:
        raise Exception ("CLR: could not serialize type: %s" % str(obj.__class__))

    ## try 1D & 2D
    etype = CLRUtils.elementTypeFor(obj)
    create = _MD.get(etype, None)
    if create is not None:
        return create(shape, obj)
    else:
        raise Exception ("CLR: could not serialize array with element type: %s" % str(etype))


##
## Initialization: obj -> message factories
##

factory = CLRTypeFactory.get()
factory.add_obj_to_msg_factory (_is_0d, _create_0d)
factory.add_obj_to_msg_factory (_is_object, _create_object)
factory.add_obj_to_msg_factory (_is_otherwise, _create_md)


##
## Intialization: message types
##

factory.register (CLRNull ())
factory.register (CLRBool())
factory.register (CLRBoolArray())
factory.register (CLRException())
factory.register (CLRFloat64())
factory.register (CLRFloat64Array())
factory.register (CLRInt32())
factory.register (CLRInt32Array())
factory.register (CLRInt64())
factory.register (CLRInt64Array())
factory.register (CLRMatrix())
factory.register (CLRObjectRef())
factory.register (CLRObjectRefArray())
factory.register (CLRString())
factory.register (CLRStringArray())
factory.register (CLRVector())

factory.register (CLRCallMethod())
factory.register (CLRCallStaticMethod())
factory.register (CLRCreateObject())
factory.register (CLRGetIndexed())
factory.register (CLRGetIndexedProperty())
factory.register (CLRGetProperty())
factory.register (CLRProtect())
factory.register (CLRRelease())
factory.register (CLRSetProperty())
factory.register (CLRTemplateReq())
factory.register (CLRTemplateReply())
