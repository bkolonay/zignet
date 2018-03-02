import React, { Component } from 'react';

import SuiteResultChart from './SuiteResultChart'
import '../common/bootstrap.css'

class SuiteResults extends Component {
  constructor(props) {
  	super();
  	this.state = { suiteResults: [] }
  }

  componentDidMount() {
  	this.props.suiteResults
  	  .then(suiteResults => {
  	  	this.setState({
  	  	  suiteResults: suiteResults
  	  	})
  	  })
  	  .catch(error => alert(error));
  }

  render() {
    return (
	  <div className="container">
	  	<div className="row">
          {this.state.suiteResults.map((suiteResult) =>
            <SuiteResultChart key={suiteResult.suiteResultId}
                              suiteResult={suiteResult} />
          )}
        </div>
      </div>
    );
  }
}

export default SuiteResults;
