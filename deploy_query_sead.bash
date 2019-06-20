#!/bin/bash
TOP_DIRECTORY=/home/roger/applications
PENDING_DIRECTORY=${TOP_DIRECTORY}/pending
PENDING_PUBLISH_DIRECTORY=${PENDING_DIRECTORY}/publish
PENDING_ZIP=${PENDING_DIRECTORY}/publish.zip
TARGET_DIRECTORY=${TOP_DIRECTORY}/publish

function start_server()
{
	pkill -f query.sead.api
	cd $TARGET_DIRECTORY
	sudo -u www-data dotnet /var/webapps/query_sead/query.sead.api.dll >> /var/log/apache2/query.sead.api.log 2>&1 &
	echo "Server started"
}

function flush_redis()
{
	redis-cli <<END_REDIS_COMMANDS
flushall
exit
END_REDIS_COMMANDS
}

if [ -d "$TARGET_DIRECTORY" ]; then
	start_server
fi
rm -rf $PENDING_PUBLISH_DIRECTORY
while true
do
	#if [ ! -d "/var/www/.dotnet" ]; then
	#	mkdir /var/www/.dotnet
	#	chown -R www-data:www-data /var/www/.dotnet/
	#fi

	if [ -f "$PENDING_ZIP" ]; then
		unzip -d $PENDING_DIRECTORY $PENDING_ZIP
		rm -f $PENDING_ZIP
	fi

	if [ -d "$PENDING_PUBLISH_DIRECTORY" ]; then
		echo "Installing new version..."
		pkill -f query.sead.api
		cd $TOP_DIRECTORY
		rm -rf $TARGET_DIRECTORY
		mv $PENDING_PUBLISH_DIRECTORY $TARGET_DIRECTORY
		chown -R www-data:www-data $TARGET_DIRECTORY
		start_server
		echo "New version installed"
	fi
	sleep 20
done

