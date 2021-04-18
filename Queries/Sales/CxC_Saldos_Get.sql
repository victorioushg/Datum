drop procedure if exists CXC_SaldosClienteRetencionesIVA;

DELIMITER //

CREATE PROCEDURE CXC_SaldosClienteRetencionesIVA(
	IN WorkId CHAR(2),
  IN CodigoCliente VARCHAR(20), 
  IN PorcentajeRetencionIVA DOUBLE(5,2)
)
BEGIN
   
   CREATE TEMPORARY TABLE tblSaldosRetIVA 
   SELECT 0 sel,  a.codcli, b.nombre, a.nummov, a.tipomov,  d.num_control, a.emision, a.vence, a.importe, 
   SUM(c.impiva) impiva, ROUND(SUM(c.impiva)*PorcentajeRetencionIVA/100,2) retimpiva 
   FROM jsventracob a 
   LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) 
   LEFT JOIN jsvenivafac c ON (a.nummov = c.numfac AND a.codcli = b.codcli AND a.id_emp = c.id_emp) 
   LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND a.codcli = d.prov_cli AND a.emision = d.emision AND 'FAC' = d.org AND 'FAC' = d.origen AND a.id_emp = d.id_emp) 
   LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.codcli = CodigoCliente AND a.id_emp = WorkID GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov)  
   WHERE 
   a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = CodigoCliente AND a.id_emp = WorkID GROUP BY a.nummov) AND  
   e.saldo <> 0.00 AND 
   d.num_control <> '' AND 
   a.tipomov = 'FC' AND 
   a.origen = 'FAC' AND 
   a.codcli = CodigoCliente AND 
   a.historico = '0' AND 
   a.ID_EMP = WorkID  
   GROUP BY nummov; 

   INSERT INTO tblSaldosRetIVA 
   SELECT 0 sel,  a.codcli, b.nombre, a.nummov, a.tipomov,  d.num_control, a.emision, a.vence, a.importe, 
   SUM(c.impiva) impiva, ROUND(SUM(c.impiva)* PorcentajeRetencionIVA /100,2) retimpiva 
   FROM jsventracob a 
   LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) 
   LEFT JOIN jsvenivapos c ON (a.nummov = c.numfac AND a.codcli = b.codcli AND a.id_emp = c.id_emp) 
   LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND a.codcli = d.prov_cli AND a.emision = d.emision AND 'PVE' = d.org AND 'FAC' = d.origen AND a.id_emp = d.id_emp) 
   LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.codcli = CodigoCliente AND a.id_emp = WorkID GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov)  
   WHERE 
   a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = CodigoCliente AND a.id_emp = WorkID GROUP BY a.nummov) AND  
   e.saldo <> 0.00 AND 
   d.num_control <> '' AND 
   a.tipomov = 'FC' AND 
   a.origen = 'PVE' AND 
   a.codcli = CodigoCliente AND
   a.historico = '0' AND 
   a.ID_EMP = WorkID 
   GROUP BY nummov; 

   INSERT INTO tblSaldosRetIVA 
   SELECT 0 sel,  a.codcli, b.nombre, a.nummov, a.tipomov,  d.num_control, a.emision, a.vence, a.importe, 
   SUM(c.impiva) impiva, ROUND(SUM(c.impiva)*PorcentajeRetencionIVA/100,2) retimpiva 
   FROM jsventracob a 
   LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) 
   LEFT JOIN jsvenivandb c ON (a.nummov = c.numndb AND a.codcli = b.codcli AND a.id_emp = c.id_emp) 
   LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND a.codcli = d.prov_cli AND a.emision = d.emision AND 'NDB' = d.org AND 'FAC' = d.origen AND a.id_emp = d.id_emp) 
   LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.codcli = CodigoCliente AND a.id_emp = WorkID GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov)  
   WHERE 
   a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = CodigoCliente AND a.id_emp = WorkID GROUP BY a.nummov) AND  
   e.saldo <> 0.00 AND 
   d.num_control <> '' AND 
   a.tipomov = 'ND' AND 
   a.origen = 'NDB' AND 
   a.codcli = CodigoCliente AND 
   a.historico = '0' AND 
   a.ID_EMP = WorkID 
   GROUP BY nummov; 

   INSERT INTO tblSaldosRetIVA
   SELECT 0 sel,  a.codcli, b.nombre, a.nummov, a.tipomov,  d.num_control, a.emision, a.vence, a.importe, 
   -1*SUM(c.impiva) impiva, -1*ROUND(SUM(c.impiva)*PorcentajeRetencionIVA/100,2) retimpiva 
   FROM jsventracob a 
   LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) 
   LEFT JOIN jsvenivancr c ON (a.nummov = c.numncr AND a.codcli = b.codcli AND a.id_emp = c.id_emp) 
   LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND a.codcli = d.prov_cli AND a.emision = d.emision AND 'NCR' = d.org AND 'FAC' = d.origen AND a.id_emp = d.id_emp) 
   LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.codcli = CodigoCliente AND a.id_emp = WorkID GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov)  
   WHERE 
   a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'ND'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = CodigoCliente AND a.id_emp = WorkID GROUP BY a.nummov) AND  
   e.saldo <> 0.00 AND 
   d.num_control <> '' AND 
   a.tipomov = 'NC' AND 
   a.origen = 'NCR' AND 
   a.codcli = CodigoCliente  AND 
   a.historico = '0' AND 
   a.ID_EMP = WorkID 
   GROUP BY nummov;

   select * from tblSaldosIVA; 
 
   drop temporary table tblSaldosIVA;
    
END //

DELIMITER ;