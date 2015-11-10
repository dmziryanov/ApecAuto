﻿USE master;
GO

CREATE ENDPOINT BrokerEndpoint
    STATE = STARTED
    AS TCP ( LISTENER_PORT = 4037 )
    FOR SERVICE_BROKER ( AUTHENTICATION = WINDOWS );
GO

ALTER DATABASE ex_rmsauto_store SET ENABLE_BROKER;
GO
