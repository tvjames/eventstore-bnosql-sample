#!/bin/bash
set -e

if [ -f "/var/vagrant_provision" ]; then 
	exit 0
fi

. /vagrant/shell/config.sh

echo "Performing provisioning..."
echo "Running as: " `whoami`

apt-get update 
apt-get install -y build-essential bison gettext pkg-config llvm

echo "Installing mono version ${MONO_VERSION}"

tar xjvf ${MONO_SRC_FILE} --directory ${MONO_TMP_DIR}

mkdir -p ${MONO_PREFIX}

cd ${MONO_TMP_DIR}/mono-${MONO_VERSION}
# LLVM is too hard, building without it for now
# ./configure --with-xen_opt=yes --prefix=${MONO_PREFIX} --enable-llvm
./configure --with-xen_opt=yes --prefix=${MONO_PREFIX} 
make
make install

date >> /etc/vagrant_provisioned_at
touch /var/vagrant_provision
