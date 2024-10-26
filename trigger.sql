CREATE TRIGGER order_insert_trigger
AFTER INSERT ON orders
FOR EACH ROW
EXECUTE FUNCTION notify_filled_order();