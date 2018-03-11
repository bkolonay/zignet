import React, { Component } from 'react';
import SuiteResultChart from './SuiteResultChart'

class SuiteResults extends Component {

  render() {
    return (
  	  <div className="container">
  	  	<div className="row">
          {this.props.suiteResults.map((suiteResult) =>
            <SuiteResultChart key={suiteResult.SuiteID}
                              suiteResult={suiteResult} />
          )}
        </div>
      </div>
    );
  }
}

export default SuiteResults;
