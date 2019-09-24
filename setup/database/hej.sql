
insert into tbl_taxa_tree_orders(order_id, date_updated, order_name, record_type_id, sort_order)
	values (139, '2015-05-22', 'Sphagnales', 2, NULL);
    
insert into tbl_taxa_tree_families(family_id, date_updated, family_name, order_id)
	values (1980, '2015-05-22', 'Sphagnaceae', 139)
    
insert into tbl_taxa_tree_genera(genus_id, date_updated, family_id, genus_name)
	values (15468, '2015-05-22', 1980, 'Sphagnum');

