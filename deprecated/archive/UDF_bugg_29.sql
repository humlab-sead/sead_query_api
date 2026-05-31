SELECT  MIN(method_values.measured_value) AS lower,
        MAX(method_values.measured_value) AS upper
FROM facet.method_measured_values(33, 0)