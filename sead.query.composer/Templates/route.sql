-- Route library table
CREATE TABLE tbl_route_definitions (
    route_name VARCHAR(100) PRIMARY KEY,
    from_table VARCHAR(100) NOT NULL,
    to_table VARCHAR(100) NOT NULL,
    route_steps JSONB NOT NULL,
    description TEXT,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Enhanced facet template table
ALTER TABLE tbl_facet_templates 
ADD COLUMN route_reference VARCHAR(100),
ADD COLUMN table_chain JSONB,
ADD COLUMN requires_distinct BOOLEAN DEFAULT FALSE;