import { parseUrlParams, stringifyUrlParams } from './UrlParameterParser.js';

function getFilter(search) {
	let urlParams = parseUrlParams(search);

	let filter = getDefaultFilter();

	if (urlParams.debug === 'true')
		filter.debug = true;
	if (urlParams.applications && urlParams.applications.includes('LoopNet'))
		filter.showLoopNet = true;
	if (urlParams.applications && urlParams.applications.includes('Listing Manager Mobile'))
		filter.showLmMobile = true;
	if (urlParams.applications && urlParams.applications.includes('CityFeet'))
		filter.showCityFeet = true;
	if (urlParams.applications && urlParams.applications.includes('Showcase'))
		filter.showShowcase = true;

	if (urlParams.environments && urlParams.environments.includes('DVM'))
		filter.showDVM = true;
	if (urlParams.environments && urlParams.environments.includes('TSM'))
		filter.showTSM = true;	
	if (urlParams.environments && urlParams.environments.includes('TSR'))
		filter.showTSR = true;		
	if (urlParams.environments && urlParams.environments.includes('Prod'))
		filter.showProd = true;			
	if (urlParams.environments && urlParams.environments.includes('Dev'))
		filter.showDev = true;			
	if (urlParams.environments && urlParams.environments.includes('Test'))
		filter.showTest = true;

	if (urlParams.suites && urlParams.suites.includes('UI'))
		filter.showUI = true;
	if (urlParams.suites && urlParams.suites.includes('Services'))
		filter.showServices = true;	

	return filter;
}

function getUrlParams(filter) {
	let urlParams = {
	  debug: false,
	  applications: [],
	  environments: [],
	  suites: []
	};

	if (filter.debug)
		urlParams.debug = true;
	if (filter.showLoopNet)
	  urlParams.applications.push('LoopNet');
	if (filter.showLmMobile)
	  urlParams.applications.push('Listing Manager Mobile');
	if (filter.showCityFeet)
	  urlParams.applications.push('CityFeet');
	if (filter.showShowcase)
	  urlParams.applications.push('Showcase');

	if (filter.showDVM)
	  urlParams.environments.push('DVM');
	if (filter.showTSM)
	  urlParams.environments.push('TSM');
	if (filter.showTSR)
	  urlParams.environments.push('TSR');
	if (filter.showProd)
	  urlParams.environments.push('Prod');
	if (filter.showDev)
	  urlParams.environments.push('Dev');	  	  	
	if (filter.showTest)
	  urlParams.environments.push('Test');

	if (filter.showUI)
	  urlParams.suites.push('UI');
	if (filter.showServices)
	  urlParams.suites.push('Services');	

	return stringifyUrlParams(urlParams);
}

function getDefaultFilter() {
	return {
		debug: false,
		showLoopNet: false,
		showLmMobile: false,
		showCityFeet: false,
		showShowcase: false,
		showDVM: false,
		showTSM: false,
		showTSR: false,
		showProd: false,
		showDev: false,
		showTest: false,
		showUI: false,
		showServices: false
	};
}

export { getFilter, getUrlParams, getDefaultFilter }