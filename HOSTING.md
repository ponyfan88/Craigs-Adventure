to host:

place in like `/var/www/html/` and hope for the best using apache2. make sure `AllowOverride` is set to `All` in your apache2 conf otherwise the redirect from `[url]/play` to `[url]/play.html` wont work.

the server at <https://www.craigsadventure.com/> uses digitalocean and godaddy so it might not work if you arent cool like us and are instead cool in your own way.