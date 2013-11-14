#!/bin/bash
set -e

if [ -f "/var/vagrant_provision_es" ]; then 
	exit 0
fi

. /vagrant/shell/config.sh

echo "Performing provisioning..."
echo "Running as: " `whoami`

echo "Installing EventStore binaries"

mkdir -p ${ES_PREFIX}
tar xzvf ${ES_SRC_FILE} --directory ${ES_PREFIX}

date >> /etc/vagrant_provisioned_at
touch /var/vagrant_provision_es
