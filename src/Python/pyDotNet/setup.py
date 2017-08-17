#!/usr/bin/env python

from distutils.core import setup, Extension
from os.path import dirname
import glob
import os
import sys
import shutil
import fnmatch
import numpy


def find_package_names (basedir='src'):
    matches = []
    for path in (path for path, sub, files in os.walk(basedir) if '__init__.py' in files):
        components = path.split('/')
        name = '.'.join(components[1:])
        matches.append (name)
    return matches


def find_package_mapping (basedir='src'):
    mapping = {}
    for path in (path for path, sub, files in os.walk(basedir) if '__init__.py' in files):
        components = path.split('/')
        name = '.'.join(components[1:])
        mapping[name] = path
    return mapping


def find_cython_files (basedir='src'):
    matches = []
    for path, sub, files in os.walk(basedir):
        for filename in fnmatch.filter(files, '*.pyx'):
            matches.append (os.path.join (path, filename))

    return matches

        
# checpyk version of python
if sys.version_info.major < 3:
    raise RuntimeError ("need to install in python 3.x, adjust path or python version")

# remove older build
if os.path.isdir ('build'):
    shutil.rmtree('build')


# copy .NET CLR server to build dir
packagedir = dirname(os.path.abspath(__file__))
targetdir = packagedir + "/build/lib/pydotnet"
srddir = dirname(dirname(packagedir)) + "/bin/Debug"

os.makedirs (targetdir)
shutil.copytree(srddir, targetdir + "/server")

# now do main setup
setup(name='pydotnet',
      version = '1.0',
      description = 'pyDotNet',
      author = 'Jonathan Shore',
      author_email = 'jonathan.shore@gmail.com',
      packages = find_package_names(),
      package_dir = find_package_mapping(),
      setup_requires = ['pandas', 'numpy'], 
      include_dirs=[numpy.get_include()]
     )

