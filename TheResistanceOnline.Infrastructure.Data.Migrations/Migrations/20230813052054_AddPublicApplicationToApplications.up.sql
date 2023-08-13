INSERT INTO Applications(ClientId, ConcurrencyToken, DisplayName, Permissions, Type, RedirectUris)
VALUES ('TheResistanceOnline.Web', LOWER(NEWID()),'The Resistance Online Public Application',
        '["ept:token","ept:authorization","gt:refresh_token","gt:authorization_code","rst:token","rst:code"]',
        'public',
        '["https://localhost:44452/user/login","https://theresistance.online/user/login","https://the-resistance-online-web.azurewebsites.net/user/login"]')