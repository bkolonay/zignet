import React, { Component } from 'react';

import SuiteResultChart from './SuiteResultChart'
import '../common/bootstrap.css'

class SuiteResults extends Component {

  render() {
  	const suiteResults = this.props.suiteResults;
    return (
	  <div className="container">
	  	<div className="row">
          {suiteResults.map((suiteResult) =>
            <SuiteResultChart key={suiteResult.suiteResultId}
                              suiteResult={suiteResult} />
          )}
        </div>
      </div>
    );
  }
}

export default SuiteResults;
