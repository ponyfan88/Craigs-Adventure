find ./html -mindepth 1 ! -regex '^./html/test\(/.*\)?' -delete
cd html
git clone https://github.com/ponyfan88/Craigs-Adventure.git

cd Craigs-Adventure

git checkout CraigGL
git switch CraigGL

cd ..
mv Craigs-Adventure/* ../*