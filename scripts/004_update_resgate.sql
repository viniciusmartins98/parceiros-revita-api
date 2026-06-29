ALTER TABLE resgates ADD COLUMN cliente_id uuid;
ALTER TABLE resgates ADD CONSTRAINT fk_resgates_cliente FOREIGN KEY (cliente_id) REFERENCES clientes (id) ON DELETE RESTRICT;
CREATE INDEX ix_resgates_cliente ON resgates (cliente_id);
-- Make parceiro_id nullable, since now a resgate can belong to either a parceiro or a cliente
ALTER TABLE resgates ALTER COLUMN parceiro_id DROP NOT NULL;