#gradlew war -PxhAppVersion=1.0.0 -PxhAppBuild=1.0.0.234

cd client-app
yarn build
xcopy "build" "C:/nginx-1.23.4/apps/hoistapp" /i /s /y

cd ..

cd nginx
copy "hoistapp.conf" "C:/nginx-1.23.4/conf" /y
copy "xh*.conf"  "C:/nginx-1.23.4/conf/includes" /y

cd ..