CREATE TABLE orders (
    id SERIAL PRIMARY KEY,
    orderid VARCHAR(50),
    ordstatus VARCHAR(20),
    price NUMERIC(10, 2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    processed BOOLEAN DEFAULT FALSE
);