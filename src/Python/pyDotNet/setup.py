#!/usr/bin/env python

from distutils.core import setup, Extension
from os.path import dirname
import subprocess
import glob
import os
import sys
import shutil
import fnmatch
import numpy


def find_package_names (basedir='src'):
    matches = []
    for path in (path for path, sub, files in os.walk(basedir) if '__init__.py' in files):
        components = path.split(os.sep)
        name = '.'.join(components[1:])
        matches.append (name)
    return matches


def find_package_mapping (basedir='src'):
    mapping = {}
    for path in (path for path, sub, files in os.walk(basedir) if '__init__.py' in files):
        components = path.split(os.sep)
        name = '.'.join(components[1:])
        mapping[name] = path
    return mapping

def which (cmd):
    syspath = os.environ['PATH']
    paths = syspath.split(os.pathsep)
    ext = ".exe" if sys.platform == 'win32' else ""
    
    for dir in paths:
        path = os.path.join (dir, cmd) + ext
        if os.path.isfile(path):
            return path

    return None

        
# check version of python
if sys.version_info.major < 3:
    raise RuntimeError ("need to install in python 3.x, adjust path or python version")

# build dirs
packagedir = dirname(os.path.abspath(__file__))
targetdir = packagedir + "/build/lib/pydotnet".replace("/", os.sep) 
srcdir = packagedir + "/server".replace("/", os.sep)

objdir = packagedir + "/server/obj".replace("/", os.sep)
bindir = packagedir + "/server/bin".replace("/", os.sep)

# remove older build
if os.path.isdir ('build'):
    shutil.rmtree('build')
if os.path.isdir (objdir):
    shutil.rmtree(objdir)
if os.path.isdir (bindir):
    shutil.rmtree(bindir)


# find .NET & .NET build commands in path
msbuild = which ("msbuild")
nuget = which ("nuget")

# try to build CLR server
if msbuild and nuget:
    cwd = os.getcwd()
    os.chdir (srcdir)
    subprocess.run ([nuget, 'restore'], stderr=subprocess.STDOUT, check=True)
    subprocess.run ([msbuild], stderr=subprocess.STDOUT, check=True)
    os.chdir (cwd)
    
    os.makedirs (targetdir)
    shutil.copytree(srcdir + "/bin/Debug".replace("/", os.sep), targetdir + "/server".replace("/", os.sep))
else:
    print ("warning: CLR server not built as could not find nuget or msbuild in the path, please add to path and rebuild")
    


# now do main setup
setup(name='pydotnet',
      version = '0.9.0',
      description = 'pyDotNet',
      author = 'Jonathan Shore',
      author_email = 'jonathan.shore@gmail.com',
      packages = find_package_names(),
      package_dir = find_package_mapping(),
      setup_requires = ['pandas', 'numpy'], 
      include_dirs=[numpy.get_include()]
     )

