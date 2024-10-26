CREATE OR REPLACE FUNCTION notify_filled_order()
RETURNS trigger AS $$
BEGIN
    IF NEW.ordstatus = 'FILLED' THEN
        -- Envia uma notificação para o canal 'filled_orders' com o ID da ordem
        PERFORM pg_notify('filled_orders', NEW.orderid);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;