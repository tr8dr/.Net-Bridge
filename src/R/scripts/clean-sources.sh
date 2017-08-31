#!/bin/sh
#
#  clean rDotNet sources
#

src_dir="rDotNet/src"
top_dir="rDotNet"

rm -f ${src_dir}/.{o,so,dll}
rm -f ${top_dir}/{build,MANIFEST}

