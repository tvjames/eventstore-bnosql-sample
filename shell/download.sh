#!/bin/bash
set -e

. /vagrant/shell/config.sh

mkdir -p /vagrant/files

if [ ! -e ${MONO_SRC_FILE} ]; then 
	# download
	wget -O - http://download.mono-project.com/sources/mono/mono-${MONO_VERSION}.tar.bz2 > ${MONO_SRC_FILE}

fi

if [ ! -e ${ES_SRC_FILE} ]; then 
	# download
	wget -O - http://download.geteventstore.com/binaries/eventstore-mono-${ES_VERSION}.tgz > ${ES_SRC_FILE}

fi
