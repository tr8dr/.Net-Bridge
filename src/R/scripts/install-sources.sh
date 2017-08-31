#!/bin/sh
#
#  Copy and compress .NET sources for the rDotNet distribution
#

lib_root="../DotNet/Library/src"
app_root="../DotNet/Server/src"

dst_dir="rDotNet/inst/server"

cat /dev/null > ${dst_dir}/Library.cs
cat /dev/null > /tmp/using

for file in `find ${lib_root} -name '*.cs'`; do
    echo "processing using directive in: ${file}"
    cat ${file} | egrep '^using' >> /tmp/using
done

echo "// -------------------------------------------" >> ${dst_dir}/Library.cs
echo "// global using directives" >> ${dst_dir}/Library.cs
echo "// -------------------------------------------" >> ${dst_dir}/Library.cs
sort /tmp/using | uniq >> ${dst_dir}/Library.cs

for file in `find ${lib_root} -name '*.cs'`; do
    echo "processing file: ${file}"
    echo "// -------------------------------------------" >> ${dst_dir}/Library.cs
    echo "// File: ${file}" >> ${dst_dir}/Library.cs
    echo "// -------------------------------------------" >> ${dst_dir}/Library.cs
    cat ${file} | egrep -v '^using' >> ${dst_dir}/Library.cs
done

echo "processing server code"
cp ${app_root}/Main.cs ${dst_dir}/Main.cs
