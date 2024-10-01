<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:output method="html"/>
	<xsl:template match="/">
		<html>
			<head>
				<title>Optrel MCA - Elaboration Parameters Report</title>
				<style type="text/css">body, table
					{
						font-family: Tahoma;
						font-size: 12px;
						color: #000000;
						background-color: #FFFFFF;
					}
					#parameters
					{
						width: 22cm;
						margin-left:auto;
						margin-right:auto;
					}
					.H1
					{
						font: 900 155% verdana, tahoma, arial, sans-serif;
						font-size: 20px;
						text-align: center;
						color: #3366FF;
						border: 1px solid #6F6F6F;
					}
					.H2
					{
						color: #3366FF;
						border: 1px solid #3366FF;
						font-size: 11px;
						font-weight: bold;
						padding: 2px;
					}
					.H21
					{
						color: #3366FF;
						border: 1px solid #3366FF;
						font-size: 10px;
						padding: 2px;
					}
					.H3
					{
						font-size: 12px;
						font-weight: bold;
						text-align: left;
					}
					.H31
					{
						font-size: 11px;
						height: 20px;
						font-weight: bold;
						text-align: right;
						padding-right: 210px
					}
					.H4
					{
						font-size: 10px;
						border: 1px solid #E1E1E1;
						height: 25px;
						width: 200px;
						padding: 1px;
					}
					.H4small
					{
						font-size: 10px;
						border: 1px solid #E1E1E1;
						height: 25px;
						width: 100px;
						padding: 1px;
					}
					.H4verysmall
					{
						font-size: 10px;
						border: 1px solid #E1E1E1;
						height: 25px;
						width: 60px;
						padding: 1px;
					}
					.H5
					{
						font-size: 11px;
						font-weight: bold;
					}
					.H5page
					{
						font-size: 10px;
						font-weight: bold;
						page-break-after: always
					}
					.note 
					{
						text-top:500;
						height:40px;
						padding:3px;
						text-align: left;
						border: 1px solid #E1E1E1;
						font-size: 11px;
						width="75%"
					}
				</style>
			</head>

			<body>
				<div align="left" id="parameters">
					<table border="0" cellspacing="0" cellpadding="2" width="75%">
						<tr>
							<td class="H1" width="30">
							<img src="ReportLogo.JPG" align="LEFT" border="0" height="60"/>	
							</td>
							<td class="H1">Elaboration Parameters</td>
						</tr>
					</table>
					<br/>
					<div class="H3">Product: 
						<xsl:apply-templates select="ProductData/product"/>
					</div>
					<div class="H3">ProductFile: 
						<xsl:apply-templates select="ProductData/productFile"/>
					</div>
					<div class="H3">Last modification:
						<xsl:apply-templates select="ProductData/date"/>
					</div>
					
					<br/>
					<xsl:for-each select="Recipe/Cams/Cam">
                                          <xsl:variable name="CamId" select="Id" />
                                          <xsl:for-each select="/Recipe/MachineConfiguration/CameraSettings/CameraSetting">
                                            <xsl:variable name="CamConfId" select="Id" />
                                            <xsl:if test="$CamConfId = $CamId">
                                              <div align="left" class="H5">ELABORATION PARAMETERS CAMERA
						<xsl:value-of select="CameraDescription"/>
					      </div>
                                            </xsl:if>
                                          </xsl:for-each>
					  <xsl:if test="count(AcquisitionParameters/AcquisitionParameter) > 0">
					  <p>
                                             <div align="left" class="H5">ACQUISITION PARAMETERS
					       <table border="0" cellspacing="0" cellpadding="0">
					         <tr align="center">
						   <td class="H2">Id</td>
						   <td class="H2">Label</td>
						   <td class="H2">Value</td>
						 </tr>
						 <xsl:apply-templates select="AcquisitionParameters/AcquisitionParameter"/>
					       </table>
					       <br/>
                                             </div>
					  </p>
                                          </xsl:if>
					  <xsl:if test="count(FeatureEnabledParameters/FeatureEnabledParameter) > 0">
					  <p>
                                             <div align="left" class="H5">FEATURED ENABLED
					       <table border="0" cellspacing="0" cellpadding="0">
					         <tr align="center">
						   <td class="H2">Id</td>
						   <td class="H2">Label</td>
						   <td class="H2">Value</td>
						 </tr>
						 <xsl:apply-templates select="FeatureEnabledParameters/FeatureEnabledParameter"/>
					       </table>
					       <br/>
                                             </div>
					  </p>
                                          </xsl:if>
					  <xsl:if test="count(RecipeSimpleParameters/RecipeSimpleParameter) > 0">
					  <p>
                                             <div align="left" class="H5">RECIPE SIMPLE PARAMETERS
					       <table border="0" cellspacing="0" cellpadding="0">
					         <tr align="center">
						   <td class="H2">Id</td>
						   <td class="H2">Label</td>
						   <td class="H2">Value</td>
						 </tr>
						 <xsl:apply-templates select="AcquisitionParameters/AcquisitionParameter"/>
					       </table>
					       <br/>
                                             </div>
					  </p>
                                          </xsl:if>
					  <xsl:if test="count(RecipeAdvancedParameters/RecipeAdvancedParameter) > 0">
					  <p>
                                             <div align="left" class="H5">RECIPE ADVANCED PARAMETERS
					       <table border="0" cellspacing="0" cellpadding="0">
					         <tr align="center">
						   <td class="H2">Id</td>
						   <td class="H2">Label</td>
						   <td class="H2">Value</td>
						 </tr>
						 <xsl:apply-templates select="RecipeAdvancedParameters/RecipeAdvancedParameter"/>
					       </table>
					       <br/>
                                             </div>
					  </p>
					  </xsl:if>						
					  <xsl:if test="count(StroboParameters/StroboParameter) > 0">
					  <p>
                                             <div align="left" class="H5">STROBO PARAMETERS
					       <table border="0" cellspacing="0" cellpadding="0">
					         <tr align="center">
						   <td class="H2">Id</td>
						   <td class="H2">Label</td>
						   <td class="H2">Value</td>
						 </tr>
						 <xsl:apply-templates select="StroboParameters/StroboParameter"/>
					       </table>
					       <br/>
                                             </div>
					  </p>
					  </xsl:if>						
					</xsl:for-each>	
					<br/>
					
					<div class="note">User signature:</div>
					<div class="note">Supervisor signature:</div>
		
				</div>
			</body>
		</html>

	</xsl:template>
	
	<!-- Template aggiuntivi per il foglio di stile -->

	<xsl:template match="AcquisitionParameter|FeatureEnabledParameter|RecipeSimpleParameter|RecipeAdvancedParameter|StroboParameter">	
		<tr align="center">
			<td class="H4verysmall">
				<xsl:value-of select="Id"/>
			</td>
			<td class="H4">
				<xsl:value-of select="Label"/>
			</td>
			<td class="H4small">
				<xsl:value-of select="Value"/>
			</td>
		</tr>	
	</xsl:template>

</xsl:stylesheet>

