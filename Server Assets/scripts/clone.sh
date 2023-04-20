# clone.sh by jack kennedy (ponyfan88)
# purpose: removes the previous build of craigs adventure from the VPS and downloads the latest
# feel free to use this in any project or whatever i dont care i just publish this to help anyone else out

find ./html -mindepth 1 ! -regex '^./html/test\(/.*\)?' -delete # delete every file in /var/www/html/ except for the "test" folder.
cd html # cd into that folder
git clone https://github.com/ponyfan88/Craigs-Adventure.git # clone craigs-adventure into /var/www/html/

cd Craigs-Adventure # cd into craigs adventure so we can use git commands

git checkout CraigGL # grab the webgl branch
git switch CraigGL # switch to said branch

cd .. # cd out
mv Craigs-Adventure/* ../* # move every file from /var/www/html/Craigs-Adventure/* to /var/www/html/* so that people actually can access it
