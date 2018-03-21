<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:math="http://www.w3.org/2005/xpath-functions/math"
    xmlns:xd="http://www.oxygenxml.com/ns/doc/xsl"
    xmlns:emp="http://www.semanticalllc.com/ns/employees#"
    xmlns:h="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    xmlns:j="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xs math xd h emp"
    version="3.0"
    expand-text="yes"
    >
  <xsl:output method="text"/>
  <xsl:strip-space elements="*"/>
  <xsl:preserve-space elements="xsl:text"/>
  <xsl:variable name="newline">
    <xsl:text></xsl:text>
  </xsl:variable>
  <xsl:variable name="tab">
    <xsl:text>&#x09;</xsl:text>
  </xsl:variable>
  <xsl:template match="sentence">
    <xsl:apply-templates select="w[@pos='NN']"></xsl:apply-templates>
  </xsl:template>
  <xsl:template match="w">
    <xsl:variable name="lemma" select="@lemma"/>
    <xsl:value-of select = "fn:replace($lemma,'a', 'b')" disable-output-escaping="yes"/>
  </xsl:template>
  
</xsl:stylesheet>