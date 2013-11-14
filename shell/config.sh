# Bash Fragment 

MONO_VERSION='3.2.3'
ES_VERSION='2.0.1'

MONO_SRC_FILE="/vagrant/files/mono-${MONO_VERSION}.tar.bz2"
MONO_TMP_DIR=`mktemp -d -t monoXXXXX`
MONO_PREFIX="/opt/mono-${MONO_VERSION}"

ES_SRC_FILE="/vagrant/files/eventstore-mono-${ES_VERSION}.tgz"
ES_PREFIX="/opt/eventstore-${ES_VERSION}"

ES_VAR="/var/eventstore"
