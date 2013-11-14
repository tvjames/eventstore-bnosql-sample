#!/bin/bash

. /vagrant/shell/config.sh

mkdir -p ${ES_VAR}/logs ${ES_VAR}/db ${ES_VAR}/run

PATH=${MONO_PREFIX}/bin:$PATH 
PKG_CONFIG_PATH=${MONO_PREFIX}/lib/pkgconfig/:$PKG_CONFIG_PATH
LD_LIBRARY_PATH=${ES_PREFIX}:$LD_LIBRARY_PATH

export LD_LIBRARY_PATH PKG_CONFIG_PATH PATH

STDFILE=`date -u +"%Y-%m-%dT%H-%M-%SZ"`
STDOUT_LOG=${ES_VAR}/run/${STDFILE}.stdout.log
STDERR_LOG=${ES_VAR}/run/${STDFILE}.stdout.log

${MONO_PREFIX}/bin/mono ${ES_PREFIX}/EventStore.SingleNode.exe --ip 0.0.0.0 --http-prefix http://*:2113/ --run-projections=all --config=/vagrant/config.json --stats-period-sec=300 --logsdir=${ES_VAR}/logs --db=${ES_VAR}/db > ${STDOUT_LOG} 2> ${STDERR_LOG} &
sleep 5
head -n 100 ${STDOUT_LOG}
tail ${STDOUT_LOG}
